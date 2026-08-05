import { Component, inject, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatTooltip } from '@angular/material/tooltip';
import { CategoryService } from '../../core/services/category.service';
import { CategoryResponseDTO, CategoryCreateDTO, CategoryUpdateDTO } from '../../shared/models/category.model';
import { PageHeader } from '../../shared/components/page-header/page-header';
import { ErrorBanner } from '../../shared/components/error-banner/error-banner';
import { EntityFormDialog, EntityDialogData,FieldConfig, } from '../../shared/components/entity-form-dialog/entity-form-dialog';
import { CircularItem } from '../../shared/components/circular-item/circular-item';
import { AppButton } from '../../shared/components/button/button';

/** Configuración de campos para el formulario de Categoría */
const CATEGORY_FIELDS: FieldConfig[] = [
  {
    key: 'name',
    label: 'Nombre',
    placeholder: 'ej. Frontend, Backend, Mobile…',
    required: true,
  },
  {
    key: 'description',
    label: 'Descripción',
    placeholder: 'Descripción breve de la categoría',
    hint: 'Opcional',
  },
];

@Component({
  selector: 'app-category-list',
  imports: [
    MatIconButton,
    MatIcon,
    MatProgressSpinner,
    MatTooltip,
    PageHeader,
    ErrorBanner,
    CircularItem,
    AppButton,
  ],
  templateUrl: './category-list.html',
  // Sin styleUrl — usa las clases globales de styles.scss
})
export class CategoryList implements OnInit {
  private readonly categoryService = inject(CategoryService);
  private readonly dialog = inject(MatDialog);

  readonly categories = this.categoryService.categories;
  readonly isLoading = this.categoryService.loading;
  readonly errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.categoryService.loadAllCategories().subscribe();
  }

  /** Abre el dialog para crear una nueva categoría */
  openCreateDialog(): void {
    const ref = this.dialog.open<EntityFormDialog, EntityDialogData>(
      EntityFormDialog,
      {
        width: '480px',
        data: {
          title: 'Nueva Categoría',
          fields: CATEGORY_FIELDS,
          submitLabel: 'Crear',
        },
      },
    );

    ref.afterClosed().subscribe((result?: Record<string, string>) => {
      if (!result) return;

      this.errorMessage.set(null);
      this.categoryService.createCategory(result as unknown as CategoryCreateDTO).subscribe({
        error: (err) => this.handleError(err, 'crear'),
      });
    });
  }

  /** Abre el dialog pre-relleno para editar una categoría existente */
  openEditDialog(category: CategoryResponseDTO): void {
    const ref = this.dialog.open<EntityFormDialog, EntityDialogData>(
      EntityFormDialog,
      {
        width: '480px',
        data: {
          title: 'Editar Categoría',
          fields: CATEGORY_FIELDS,
          initialData: { name: category.name, description: category.description },
          submitLabel: 'Guardar cambios',
        },
      },
    );

    ref.afterClosed().subscribe((result?: Record<string, string>) => {
      if (!result) return;

      this.errorMessage.set(null);
      this.categoryService.updateCategory(category.categoryId, result as unknown as CategoryUpdateDTO).subscribe({
        error: (err) => this.handleError(err, 'actualizar'),
      });
    });
  }

  /** Elimina una categoría con confirmación básica */
  deleteCategory(category: CategoryResponseDTO): void {
    const confirmed = confirm(
      `¿Eliminar la categoría "${category.name}"?\nSolo se puede eliminar si no tiene proyectos asociados.`,
    );
    if (!confirmed) return;

    this.errorMessage.set(null);
    this.categoryService.deleteCategory(category.categoryId).subscribe({
      error: (err) => {
        const msg = err.error?.mensaje ?? 'No se pudo eliminar la categoría.';
        this.errorMessage.set(msg);
      },
    });
  }

  private handleError(err: any, action: string): void {
    const msg =
      err.error?.mensaje ??
      err.error?.title ??
      `No se pudo ${action} la categoría.`;
    this.errorMessage.set(msg);
  }
}
