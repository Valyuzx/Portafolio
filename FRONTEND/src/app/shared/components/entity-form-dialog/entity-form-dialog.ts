import { Component, inject, OnInit, signal } from '@angular/core';
import {
  MAT_DIALOG_DATA,
  MatDialogRef,
  MatDialogTitle,
  MatDialogContent,
  MatDialogActions,
} from '@angular/material/dialog';
import { FormBuilder, FormGroup, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { MatFormField, MatLabel, MatError, MatHint } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { ErrorBanner } from '../error-banner/error-banner';
import { AppButton } from '../button/button';

/** Configuración de un campo individual del formulario dinámico */
export interface FieldConfig {
  key: string;
  label: string;
  placeholder?: string;
  required?: boolean;
  type?: 'text' | 'url';
  hint?: string;
}

/** Datos que recibe el dialog al abrirse */
export interface EntityDialogData {
  title: string;
  fields: FieldConfig[];
  initialData?: Record<string, string | null | undefined>;
  submitLabel?: string;
  loadingLabel?: string;
}


@Component({
  selector: 'app-entity-form-dialog',
  imports: [
    ReactiveFormsModule,
    MatDialogTitle,
    MatDialogContent,
    MatDialogActions,
    MatFormField,
    MatLabel,
    MatError,
    MatHint,
    MatInput,
    ErrorBanner,
    AppButton,
  ],
  template: `
    <h2 mat-dialog-title class="dialog-title">{{ data.title }}</h2>

    <mat-dialog-content class="dialog-body">
      <form [formGroup]="form" novalidate class="dialog-form">

        @for (field of data.fields; track field.key) {
          <mat-form-field appearance="outline" class="field-full">
            <mat-label>{{ field.label }}</mat-label>

            <input
              matInput
              [formControlName]="field.key"
              [type]="field.type ?? 'text'"
              [placeholder]="field.placeholder ?? ''"
            />

            @if (field.hint) {
              <mat-hint>{{ field.hint }}</mat-hint>
            }

            @if (form.get(field.key)?.hasError('required')) {
              <mat-error>{{ field.label }} es obligatorio</mat-error>
            }
          </mat-form-field>
        }

        <app-error-banner [message]="errorMessage()" />

      </form>
    </mat-dialog-content>

    <mat-dialog-actions align="end" class="dialog-actions">
      <app-button variant="secondary" type="button" (click)="onCancel()">
        Cancelar
      </app-button>

      <app-button
        type="button"
        [disabled]="isLoading() || form.invalid"
        [loading]="isLoading()"
        [loadingLabel]="data.loadingLabel ?? 'Guardando...'"
        icon="save"
        (click)="onSubmit()"
      >
        {{ data.submitLabel ?? 'Guardar' }}
      </app-button>
    </mat-dialog-actions>
  `,
  styles: `
    .dialog-title {
      color: var(--app-text-primary);
      font: var(--mat-sys-title-large);
      margin: 0;
      padding: 1.25rem 1.5rem 0.5rem;
    }

    .dialog-body {
      padding: 0.5rem 1.5rem 0;
      overflow: visible;
    }

    .dialog-form {
      display: flex;
      flex-direction: column;
      gap: 0.25rem;
      padding-top: 0.5rem;
    }

    .field-full { width: 100%; }

    .dialog-actions {
      padding: 1rem 1.5rem 1.25rem;
      gap: 0.75rem;
    }
  `,
})
export class EntityFormDialog implements OnInit {
  readonly data = inject<EntityDialogData>(MAT_DIALOG_DATA);
  readonly dialogRef = inject(MatDialogRef<EntityFormDialog>);
  private readonly fb = inject(FormBuilder);

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  form!: FormGroup;

  ngOnInit(): void {
    const controls: Record<string, [string, ValidatorFn[]]> = {};

    for (const field of this.data.fields) {
      const initial = this.data.initialData?.[field.key] ?? '';
      controls[field.key] = [
        initial as string,
        field.required ? [Validators.required] : [],
      ];
    }

    this.form = this.fb.nonNullable.group(controls);
  }

  onSubmit(): void {
    if (this.form.invalid) return;
    this.dialogRef.close(this.form.getRawValue());
  }

  onCancel(): void {
    this.dialogRef.close(undefined);
  }

  setError(message: string): void {
    this.errorMessage.set(message);
    this.isLoading.set(false);
  }
}
