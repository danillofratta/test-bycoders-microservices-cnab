import { Component, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-upload',
   imports: [CommonModule, FormsModule],
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent {
  private http = inject(HttpClient);
  private base = environment.apiBase;
  
  file = signal<File | null>(null);
  status = signal('');

  onFile(e: Event): void {
    const input = e.target as HTMLInputElement;
    this.file.set(input.files && input.files.length ? input.files[0] : null);
  }

  send(): void {
    const currentFile = this.file();
    if (!currentFile) {
      this.status.set('Nenhum arquivo selecionado');
      return;
    }

    this.status.set('Enviando...');
    const formData = new FormData();
    formData.append('file', currentFile);

    this.http.post(`${this.base}/upload`, formData).subscribe({
      next: (response: any) => {
        this.status.set(JSON.stringify(response, null, 2));
      },
      error: (err: any) => {
        console.error('Erro no upload:', err);
        this.status.set('Erro: ' + (err?.error?.message || err?.message || 'Erro desconhecido'));
      }
    });
  }
}
