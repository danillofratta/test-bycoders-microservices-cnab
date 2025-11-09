import { Component, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Component({
  standalone: true,
  selector: 'app-upload',
  template: `
    <section style="background:#111827;border:1px solid #1f2937;border-radius:12px;padding:16px">
      <h2>Upload CNAB.txt</h2>
      <input 
        type="file" 
        (change)="onFile($event)" 
        accept=".txt"
      />
      <button 
        (click)="send()" 
        [disabled]="!file()"
      >
        Upload & Publicar
      </button>
      <pre style="color:#9ca3af">{{ status() }}</pre>
    </section>
  `
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
    if (!this.file()) {
      return;
    }

    this.status.set('Enviando...');
    const formData = new FormData();
    formData.append('file', this.file()!);

    this.http.post(`${this.base}/upload`, formData).subscribe({
      next: (response: any) => {
        this.status.set(JSON.stringify(response, null, 2));
      },
      error: (err: any) => {
        this.status.set('Erro: ' + (err?.message || err));
      }
    });
  }
}
