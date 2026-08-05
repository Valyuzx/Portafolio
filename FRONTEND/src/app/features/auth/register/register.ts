import { Component, computed, inject, signal } from '@angular/core';
import {
  AbstractControl,
  FormBuilder,
  ReactiveFormsModule,
  ValidationErrors,
  Validators,
} from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { toSignal } from '@angular/core/rxjs-interop';
import { MatFormField, MatLabel, MatError, MatSuffix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { AuthService } from '../../../core/services/auth.service';
import { AuthLayout } from '../../../shared/components/auth-layout/auth-layout';
import { PasswordField } from '../../../shared/components/password-field/password-field';
import { ErrorBanner } from '../../../shared/components/error-banner/error-banner';
import { AppButton } from '../../../shared/components/button/button';

function passwordMatchValidator(group: AbstractControl): ValidationErrors | null {
  const password = group.get('password')?.value ?? '';
  const confirm = group.get('confirmPassword')?.value ?? '';
  return password === confirm ? null : { passwordMismatch: true };
}

@Component({
  selector: 'app-register',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatFormField,
    MatLabel,
    MatError,
    MatSuffix,
    MatInput,
    MatIcon,
    AuthLayout,
    PasswordField,
    ErrorBanner,
    AppButton,
  ],
  templateUrl: './register.html',
})
export class Register {
  private readonly fb = inject(FormBuilder);
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  readonly form = this.fb.group(
    {
      userName: ['', [Validators.required, Validators.maxLength(100)]],
      email: ['', [Validators.required, Validators.email, Validators.maxLength(255)]],
      password: ['', [Validators.required, Validators.minLength(8)]],
      confirmPassword: ['', [Validators.required]],
    },
    { validators: passwordMatchValidator },
  );

  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  private readonly passwordValue = toSignal(
    this.form.controls.password.valueChanges,
    { initialValue: '' },
  );

  private readonly confirmValue = toSignal(
    this.form.controls.confirmPassword.valueChanges,
    { initialValue: '' },
  );

  readonly requirements = computed(() => {
    const pw = this.passwordValue() ?? '';
    const confirm = this.confirmValue() ?? '';
    return {
      hasNumber: /\d/.test(pw),
      hasUppercase: /[A-Z]/.test(pw),
      hasLowercase: /[a-z]/.test(pw),
      hasMinLength: pw.length >= 8,
      passwordsMatch: pw.length > 0 && pw === confirm,
    };
  });

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const { userName, email, password } = this.form.getRawValue();

    this.authService
      .register({ userName: userName!, email: email!, password: password! })
      .subscribe({
        next: () => this.router.navigate(['/login']),
        error: (err) => {
          this.errorMessage.set(
            err.status === 409
              ? 'El correo electrónico ya está registrado.'
              : 'Ocurrió un error. Intenta de nuevo.',
          );
          this.isLoading.set(false);
        },
        complete: () => this.isLoading.set(false),
      });
  }
}
