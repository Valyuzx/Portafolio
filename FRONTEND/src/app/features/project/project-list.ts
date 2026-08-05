import { Component, inject, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { DatePipe } from '@angular/common';
import {
  MatCard, MatCardHeader, MatCardTitle, MatCardSubtitle,
  MatCardContent, MatCardActions,
} from '@angular/material/card';
import { MatButton, MatIconButton } from '@angular/material/button';
import { MatIcon } from '@angular/material/icon';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MatChipSet, MatChip } from '@angular/material/chips';
import { MatTooltip } from '@angular/material/tooltip';
import { ProjectService } from './services/project.service';
import { PageHeader } from '../../shared/components/page-header/page-header';
import { AppButton } from '../../shared/components/button/button';

@Component({
  selector: 'app-project-list',
  imports: [
    RouterLink,
    DatePipe,
    MatCard, MatCardHeader, MatCardTitle, MatCardSubtitle, MatCardContent, MatCardActions,
    MatIconButton,
    MatIcon,
    MatProgressSpinner,
    MatChipSet, MatChip,
    MatTooltip,
    PageHeader,
    AppButton,
  ],
  templateUrl: './project-list.html',
  styleUrl: './project-list.scss',
})
export class ProjectList implements OnInit {
  private readonly projectService = inject(ProjectService);

  readonly projects = this.projectService.projects;
  readonly isLoading = this.projectService.loading;

  ngOnInit(): void {
    this.projectService.loadAllProjects().subscribe();
  }

  deleteProject(id: number): void {
    if (confirm('¿Estás seguro de que deseas eliminar este proyecto? Esta acción no se puede deshacer.')) {
      // TODO: implementar delete
    }
  }
}