import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiTransaction } from '../../../../../domain/api/api-transaction';
import { GetAllTransactionsResponse } from '../../../../../domain/dto/transaction/get-list/get-all-transaction-response';

@Component({
  selector: 'app-transaction-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './transaction-list.component.html',
  styleUrls: ['./transaction-list.component.css']
})
export class TransactionListComponent implements OnInit {
  transactions: GetAllTransactionsResponse[] = [];
  loading: boolean = false;
  error: string = '';

  constructor(private apiTransaction: ApiTransaction) { }

  async ngOnInit(): Promise<void> {
    await this.loadTransactions();
  }

  async loadTransactions(): Promise<void> {
    try {
      this.loading = true;
      this.error = '';
      
      const observable = await this.apiTransaction.GetListAll();
      observable.subscribe({
        next: (data: GetAllTransactionsResponse[]) => {
          this.transactions = data;
          this.loading = false;
        },
        error: (err: any) => {
          console.error('Erro ao carregar transações:', err);
          this.error = 'Erro ao carregar transações. Tente novamente.';
          this.loading = false;
        }
      });
    } catch (error) {
      console.error('Erro ao carregar transações:', error);
      this.error = 'Erro ao carregar transações. Tente novamente.';
      this.loading = false;
    }
  }

  async refreshTransactions(): Promise<void> {
    await this.loadTransactions();
  }

  formatCurrency(value: number): string {
    return new Intl.NumberFormat('pt-BR', {
      style: 'currency',
      currency: 'BRL'
    }).format(value);
  }

  // Método para usar o valor com sinal (positivo/negativo) correto
  getCorrectValue(transaction: GetAllTransactionsResponse): number {
    return transaction.signedValue;
  }

  formatDate(date: Date | string): string {
    if (!date) return '';
    const dateObj = typeof date === 'string' ? new Date(date) : date;
    return new Intl.DateTimeFormat('pt-BR', {
      day: '2-digit',
      month: '2-digit',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    }).format(dateObj);
  }

  trackByTransactionId(index: number, transaction: GetAllTransactionsResponse): number {
    return transaction.id;
  }
}