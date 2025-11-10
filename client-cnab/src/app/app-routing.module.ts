import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { UploadComponent } from './pages/dashboard/transaction/upload/upload.component';

const routes: Routes =
  [
    {
      path: '',
      //canActivate: [AuthService],
      component: DashboardComponent,      
      children: [
        { path: 'Upload', component: UploadComponent },
        { path: '', redirectTo: 'dashboard', pathMatch: 'full' } // Rota padrão
      ]
    }
  ];  

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
