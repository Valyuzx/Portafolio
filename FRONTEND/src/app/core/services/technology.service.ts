import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api.constants';
import {
  TechnologyResponseDTO,
  TechnologyCreateDTO,
  TechnologyUpdateDTO,
} from '../../shared/models/technology.model';

@Injectable({ providedIn: 'root' })
export class TechnologyService {
  private readonly http = inject(HttpClient);

  private readonly _technologies = signal<TechnologyResponseDTO[]>([]);
  private readonly _loading      = signal<boolean>(false);

  readonly technologies = this._technologies.asReadonly();
  readonly loading      = this._loading.asReadonly();

  loadAllTechnologies() {
    this._loading.set(true);
    return this.http
      .get<TechnologyResponseDTO[]>(API_ENDPOINTS.technologies.list)
      .pipe(
        tap({
          next:     (data) => this._technologies.set(data),
          finalize: ()     => this._loading.set(false),
        }),
      );
  }

  createTechnology(dto: TechnologyCreateDTO) {
    return this.http
      .post<TechnologyResponseDTO>(API_ENDPOINTS.technologies.create, dto)
      .pipe(
        tap((created) =>
          this._technologies.update((techs) => [...techs, created]),
        ),
      );
  }

  updateTechnology(id: string, dto: TechnologyUpdateDTO) {
    return this.http
      .put<TechnologyResponseDTO>(API_ENDPOINTS.technologies.update(id), dto)
      .pipe(
        tap((updated) =>
          this._technologies.update((techs) =>
            techs.map((t) => (t.technologyId === id ? updated : t)),
          ),
        ),
      );
  }

  deleteTechnology(id: string) {
    return this.http
      .delete<void>(API_ENDPOINTS.technologies.delete(id))
      .pipe(
        tap(() =>
          this._technologies.update((techs) =>
            techs.filter((t) => t.technologyId !== id),
          ),
        ),
      );
  }
}