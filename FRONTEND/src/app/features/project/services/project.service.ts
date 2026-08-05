import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { API_ENDPOINTS } from '../../../core/constants/api.constants';
import { ProjectResponseDTO, ProjectCreateDTO, ProjectUpdateDTO } from '../../../shared/models/project.model';

@Injectable({ providedIn: 'root' })

export class ProjectService {
  private readonly http = inject(HttpClient);

  private readonly _projects = signal<ProjectResponseDTO[]>([]);
  private readonly _loading = signal<boolean>(false);
  readonly projects = this._projects.asReadonly();
  readonly loading = this._loading.asReadonly();
  
  readonly publishedProjects = computed(() => 
    this._projects().filter(p => p.isPublished)
  );
  
  loadAllProjects() {
    this._loading.set(true);
    return this.http.get<ProjectResponseDTO[]>(API_ENDPOINTS.projects.list).pipe(
      tap({
        next: (data) => this._projects.set(data),
        finalize: () => this._loading.set(false)
      })
    );
  }

   createProject(data: ProjectCreateDTO) {
    return this.http.post<ProjectResponseDTO>(API_ENDPOINTS.projects.create, data).pipe(
      tap((newProject) => {
        this._projects.update(projects => [...projects, newProject]);
      })
    );
  }
}