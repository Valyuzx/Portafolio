export interface TechnologyResponseDTO {
  technologyId: string; 
  name: string;
  iconUrl?: string;     
}
export interface TechnologyCreateDTO {
  name: string;
  iconUrl?: string;
}
export type TechnologyUpdateDTO = Partial<TechnologyCreateDTO>;