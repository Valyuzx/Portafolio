import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';
import { noAuthGuard } from './core/guards/no-auth.guard';

export const routes: Routes = [
  {
    path: 'login',
    canActivate: [noAuthGuard],
    loadComponent: () =>
      import('./features/auth/login/login').then(m => m.Login),
  },
  {
    path: 'register',
    canActivate: [noAuthGuard],
    loadComponent: () =>
      import('./features/auth/register/register').then(m => m.Register),
  },
  {
    path: 'home',
    canActivate: [authGuard],
    loadComponent: () =>import('./features/home/home').then(m => m.Home),
    children: [
      {
        path: '', 
        loadComponent: () => import('./features/home/dashboard/dashboard').then(m => m.Dashboard)
      },
      {
        path: 'projects',
        loadComponent: () => import('./features/project/project-list').then(m => m.ProjectList),
        
      },
      {
        path: 'projects/create',
        loadComponent: () =>
          import('./features/project/project-create/project-create').then(m => m.ProjectCreate),
      },
      {
        path: 'categories',
        loadComponent: () =>
          import('./features/category/category-list').then(m => m.CategoryList),
      },
      {
        path: 'technologies',
        loadComponent: () =>
          import('./features/technology/technology-list').then(m => m.TechnologyList),
      },
    ]
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login' },
];
