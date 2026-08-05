import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api.constants';
import {
  CategoryResponseDTO,
  CategoryCreateDTO,
  CategoryUpdateDTO,
} from '../../shared/models/category.model';

@Injectable({ providedIn: 'root' })
export class CategoryService {
  private readonly http = inject(HttpClient);

  private readonly _categories = signal<CategoryResponseDTO[]>([]);
  private readonly _loading    = signal<boolean>(false);

  readonly categories = this._categories.asReadonly();
  readonly loading    = this._loading.asReadonly();

  loadAllCategories() {
    this._loading.set(true);
    return this.http.get<CategoryResponseDTO[]>(API_ENDPOINTS.categories.list).pipe(
      tap({
        next:     (data) => this._categories.set(data),
        finalize: ()     => this._loading.set(false),
      }),
    );
  }

  createCategory(dto: CategoryCreateDTO) {
    return this.http
      .post<CategoryResponseDTO>(API_ENDPOINTS.categories.create, dto)
      .pipe(
        tap((created) =>
          this._categories.update((cats) => [...cats, created]),
        ),
      );
  }

  updateCategory(id: string, dto: CategoryUpdateDTO) {
    return this.http
      .put<CategoryResponseDTO>(API_ENDPOINTS.categories.update(id), dto)
      .pipe(
        tap((updated) =>
          this._categories.update((cats) =>
            cats.map((c) => (c.categoryId === id ? updated : c)),
          ),
        ),
      );
  }

  deleteCategory(id: string) {
    return this.http
      .delete<void>(API_ENDPOINTS.categories.delete(id))
      .pipe(
        tap(() =>
          this._categories.update((cats) =>
            cats.filter((c) => c.categoryId !== id),
          ),
        ),
      );
  }
}