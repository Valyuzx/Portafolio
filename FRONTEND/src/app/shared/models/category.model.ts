export interface CategoryResponseDTO {
  categoryId: string; 
  name: string;
  description?: string; 
}
export interface CategoryCreateDTO {
  name: string;
  description?: string;
}
export type CategoryUpdateDTO = Partial<CategoryCreateDTO>;