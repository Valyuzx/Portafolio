import { environment } from '../../../enviroment/enviroment.development';

const BASE          = `${environment.backendUrl}/api/v1/accounts`;
const BASE_PROJECTS = `${environment.backendUrl}/api/v1/proyectos`;
const BASE_CATS     = `${environment.backendUrl}/api/v1/categories`;
const BASE_TECHS    = `${environment.backendUrl}/api/v1/technologies`;

export const API_ENDPOINTS = {
  auth: {
    login:    `${BASE}/login`,
    register: `${BASE}/register`,
    refresh:  `${BASE}/refresh`,
    logout:   `${BASE}/logout`,
  },
  projects: {
    list:   `${BASE_PROJECTS}/all`,
    public: `${BASE_PROJECTS}`,
    create: `${BASE_PROJECTS}/register`,
    detail: (id: string) => `${BASE_PROJECTS}/${id}`,
    update: (id: string) => `${BASE_PROJECTS}/${id}`,
    delete: (id: string) => `${BASE_PROJECTS}/${id}`,
  },
  categories: {
    list:   `${BASE_CATS}`,
    create: `${BASE_CATS}/register`,
    update: (id: string) => `${BASE_CATS}/${id}`,
    delete: (id: string) => `${BASE_CATS}/${id}`,
  },
  technologies: {
    list:   `${BASE_TECHS}`,
    create: `${BASE_TECHS}/register`,
    update: (id: string) => `${BASE_TECHS}/${id}`,
    delete: (id: string) => `${BASE_TECHS}/${id}`,
  },
} as const;
