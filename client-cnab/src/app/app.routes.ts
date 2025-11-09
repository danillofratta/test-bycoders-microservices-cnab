import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: '/upload', pathMatch: 'full' },
  { path: 'upload', loadComponent: () => import('./upload/upload.component').then(m => m.UploadComponent) },
  { path: 'stores', loadComponent: () => import('./stores/stores.component').then(m => m.StoresComponent) },
  { path: '**', redirectTo: '/upload' }
];
