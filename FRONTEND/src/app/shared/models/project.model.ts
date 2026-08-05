export interface ProjectResponseDTO {
  id: string;
  title: string;
  resume: string;
  description: string;
  repositoryURL: string;
  principalImageUrl: string;
  developmentDate: string;
  isPublished: boolean;
  categoryId: string;
  categoryName?: string;
  technologies?: any[];
}

export interface ProjectCreateDTO {
  title: string;
  resume: string;
  description: string;
  repositoryURL?: string | null;
  principalImageUrl: string;
  developmentDate: string;
  isPublished: boolean;
  categoryId: string;
  technologyIds: string[];
}

export type ProjectUpdateDTO = Partial<ProjectCreateDTO>;