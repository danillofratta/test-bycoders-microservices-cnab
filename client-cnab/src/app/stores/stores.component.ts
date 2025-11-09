import { Component, inject, signal, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { environment } from '../../environments/environment';

interface Store {
  name: string;
  balance: number;
}

@Component({
  standalone: true,
  selector: 'app-stores',
  imports: [CommonModule],
  template: `
    <section style="background:#111827;border:1px solid #1f2937;border-radius:12px;padding:16px">
      <h2>Lojas e Saldo</h2>
      <button (click)="load()">Recarregar</button>
      <table style="width:100%;margin-top:12px;border-collapse:collapse">
        <thead>
          <tr>
            <th style="text-align:left">Loja</th>
            <th style="text-align:right">Saldo</th>
          </tr>
        </thead>
        <tbody>
          <tr *ngFor="let store of stores()">
            <td>{{ store.name }}</td>
            <td style="text-align:right">{{ store.balance | number:'1.2-2' }}</td>
          </tr>
        </tbody>
      </table>
    </section>
  `
})
export class StoresComponent implements OnInit {
  private http = inject(HttpClient);
  private base = environment.apiBase;
  
  stores = signal<Store[]>([]);

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.http.get<Store[]>(`${this.base}/stores`).subscribe({
      next: (response: Store[]) => {
        this.stores.set(response);
      },
      error: (err: any) => {
        console.error('Erro ao carregar lojas:', err);
      }
    });
  }
}
