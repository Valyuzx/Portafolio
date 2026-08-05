import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { MatFormField, MatLabel, MatError, MatPrefix } from '@angular/material/form-field';
import { MatInput } from '@angular/material/input';
import { MatIcon } from '@angular/material/icon';
import { MatSelect, MatOption } from '@angular/material/select';
import { MatSlideToggle } from '@angular/material/slide-toggle';
import { ProjectService } from '../services/project.service';
import { CategoryService } from '../../../core/services/category.service';
import { TechnologyService } from '../../../core/services/technology.service';
import { ErrorBanner } from '../../../shared/components/error-banner/error-banner';
import { PageHeader } from '../../../shared/components/page-header/page-header';
import { FormSection } from '../../../shared/components/form-section/form-section';
import { AppButton } from '../../../shared/components/button/button';

@Component({
  selector: 'app-project-create',
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatFormField,
    MatLabel,
    MatError,
    MatPrefix,
    MatInput,
    MatIcon,
    MatSelect,
    MatOption,
    MatSlideToggle,
    ErrorBanner,
    PageHeader,
    FormSection,
    AppButton,
  ],
  templateUrl: './project-create.html',
  styleUrl: './project-create.scss',
})
export class ProjectCreate implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly projectService = inject(ProjectService);
  private readonly categoryService = inject(CategoryService);
  private readonly techService = inject(TechnologyService);
  private readonly router = inject(Router);

  readonly categories = this.categoryService.categories;
  readonly technologies = this.techService.technologies;
  readonly isLoading = signal(false);
  readonly errorMessage = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    title: ['', Validators.required],
    resume: ['', Validators.required],
    description: ['', Validators.required],
    repositoryURL: [''],
    principalImageUrl: ['', Validators.required],
    developmentDate: [new Date().toISOString().split('T')[0], Validators.required],
    isPublished: [false],
    categoryId: ['', Validators.required],
    technologyIds: [[] as string[], Validators.required],
  });

  ngOnInit(): void {
    this.categoryService.loadAllCategories().subscribe();
    this.techService.loadAllTechnologies().subscribe();
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.isLoading.set(true);
    this.errorMessage.set(null);

    const rawValue = this.form.getRawValue();
    const payload = {
      ...rawValue,
      repositoryURL: rawValue.repositoryURL || null,
    };

    this.projectService.createProject(payload).subscribe({
      next: () => this.router.navigate(['/home/projects']),
      error: (err) => {
        const serverMsg =
          err.error?.mensaje || err.error?.title || JSON.stringify(err.error?.errors);
        this.errorMessage.set(`Error: ${serverMsg || 'Revisa la consola'}`);
        this.isLoading.set(false);
      },
      complete: () => this.isLoading.set(false),
    });
  }
}
