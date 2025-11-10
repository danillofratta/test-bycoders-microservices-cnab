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
  templateUrl: './stores.component.html',
  styleUrls: ['./stores.component.css']
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
        this.stores.set(response || []);
      },
      error: (err: any) => {
        console.error('Erro ao carregar lojas:', err);
        // Em caso de erro, definir um array vazio para não quebrar a UI
        this.stores.set([]);
      }
    });
  }
}
