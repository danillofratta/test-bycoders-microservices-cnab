import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiStore } from '../../../../../domain/api/api-store';
import { GetAllStoresResponse } from '../../../../../domain/dto/store/get-list/get-all-store-response';

@Component({
  selector: 'app-store-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './store-list.component.html',
  styleUrls: ['./store-list.component.css']
})
export class StoreListComponent implements OnInit {
  stores: GetAllStoresResponse[] = [];
  loading: boolean = false;
  error: string = '';

  constructor(private apiStore: ApiStore) { }

  async ngOnInit(): Promise<void> {
    await this.loadStores();
  }

  async loadStores(): Promise<void> {
    try {
      this.loading = true;
      this.error = '';
      
      const observable = await this.apiStore.GetListAll();
      observable.subscribe({
        next: (data: GetAllStoresResponse[]) => {
          this.stores = data;
          this.loading = false;
        },
        error: (err: any) => {
          console.error('Erro ao carregar lojas:', err);
          this.error = 'Erro ao carregar lojas. Tente novamente.';
          this.loading = false;
        }
      });
    } catch (error) {
      console.error('Erro ao carregar lojas:', error);
      this.error = 'Erro ao carregar lojas. Tente novamente.';
      this.loading = false;
    }
  }

  async refreshStores(): Promise<void> {
    await this.loadStores();
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  }

  trackByStoreName(index: number, store: GetAllStoresResponse): string {
    return store.name;
  }

  getBalanceClass(balance: number): string {
    return balance >= 0 ? 'positive' : 'negative';
  }
}