import { Component, inject, OnInit, signal } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatTooltip } from '@angular/material/tooltip';
import { TechnologyService } from '../../core/services/technology.service';
import { TechnologyResponseDTO, TechnologyCreateDTO, TechnologyUpdateDTO } from '../../shared/models/technology.model';
import { PageHeader } from '../../shared/components/page-header/page-header';
import { ErrorBanner } from '../../shared/components/error-banner/error-banner';
import {
  EntityFormDialog,
  EntityDialogData,
  FieldConfig,
} from '../../shared/components/entity-form-dialog/entity-form-dialog';
import { CircularItem } from '../../shared/components/circular-item/circular-item';
import { AppButton } from '../../shared/components/button/button';

/** Configuración de campos para el formulario de Tecnología */
const TECHNOLOGY_FIELDS: FieldConfig[] = [
  {
    key: 'name',
    label: 'Nombre',
    placeholder: 'ej. Angular, .NET, PostgreSQL…',
    required: true,
  },
  {
    key: 'iconUrl',
    label: 'URL del ícono',
    placeholder: 'https://cdn.example.com/angular.svg',
    type: 'url',
    hint: 'Opcional — SVG o PNG del logo de la tecnología',
  },
];

@Component({
  selector: 'app-technology-list',
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
  templateUrl: './technology-list.html',
  // Sin styleUrl — usa las clases globales de styles.scss
})
export class TechnologyList implements OnInit {
  private readonly techService = inject(TechnologyService);
  private readonly dialog = inject(MatDialog);

  readonly technologies = this.techService.technologies;
  readonly isLoading = this.techService.loading;
  readonly errorMessage = signal<string | null>(null);

  ngOnInit(): void {
    this.techService.loadAllTechnologies().subscribe();
  }

  openCreateDialog(): void {
    const ref = this.dialog.open<EntityFormDialog, EntityDialogData>(
      EntityFormDialog,
      {
        width: '480px',
        data: {
          title: 'Nueva Tecnología',
          fields: TECHNOLOGY_FIELDS,
          submitLabel: 'Crear',
        },
      },
    );

    ref.afterClosed().subscribe((result?: Record<string, string>) => {
      if (!result) return;

      this.errorMessage.set(null);
      this.techService.createTechnology(result as unknown as TechnologyCreateDTO).subscribe({
        error: (err) => this.handleError(err, 'crear'),
      });
    });
  }

  openEditDialog(tech: TechnologyResponseDTO): void {
    const ref = this.dialog.open<EntityFormDialog, EntityDialogData>(
      EntityFormDialog,
      {
        width: '480px',
        data: {
          title: 'Editar Tecnología',
          fields: TECHNOLOGY_FIELDS,
          initialData: { name: tech.name, iconUrl: tech.iconUrl },
          submitLabel: 'Guardar cambios',
        },
      },
    );

    ref.afterClosed().subscribe((result?: Record<string, string>) => {
      if (!result) return;

      this.errorMessage.set(null);
      this.techService.updateTechnology(tech.technologyId, result as unknown as TechnologyUpdateDTO).subscribe({
        error: (err) => this.handleError(err, 'actualizar'),
      });
    });
  }

  deleteTechnology(tech: TechnologyResponseDTO): void {
    const confirmed = confirm(
      `¿Eliminar la tecnología "${tech.name}"?\nSolo se puede eliminar si no tiene proyectos asociados.`,
    );
    if (!confirmed) return;

    this.errorMessage.set(null);
    this.techService.deleteTechnology(tech.technologyId).subscribe({
      error: (err) => {
        const msg = err.error?.mensaje ?? 'No se pudo eliminar la tecnología.';
        this.errorMessage.set(msg);
      },
    });
  }

  private handleError(err: any, action: string): void {
    const msg =
      err.error?.mensaje ??
      err.error?.title ??
      `No se pudo ${action} la tecnología.`;
    this.errorMessage.set(msg);
  }
}
