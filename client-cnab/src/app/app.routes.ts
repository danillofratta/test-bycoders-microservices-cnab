import { Routes } from '@angular/router';
import { UploadComponent } from './pages/dashboard/transaction/upload/upload.component';
import { TransactionListComponent } from './pages/dashboard/transaction/list/transaction-list.component';
import { StoreListComponent } from './pages/dashboard/store/list/store-list.component';

export const routes: Routes = [
  { path: '', redirectTo: '/upload', pathMatch: 'full' },
  { path: 'upload', component: UploadComponent },
  { path: 'transactions', component: TransactionListComponent },
  { path: 'transactions/list', component: TransactionListComponent },
  { path: 'stores', component: StoreListComponent },
  { path: 'stores/list', component: StoreListComponent },
  { path: '**', redirectTo: '/upload' }
];