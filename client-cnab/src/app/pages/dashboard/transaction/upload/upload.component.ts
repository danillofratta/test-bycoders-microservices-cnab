
import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpEventType, HttpResponse } from '@angular/common/http';
import { ApiTransaction } from '../../../../../domain/api/api-transaction';

interface UploadStatus {
  type: 'success' | 'error';
  message: string;
}

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  public selectedFile: File | null = null;
  public isUploading: boolean = false;
  public uploadProgress: number = 0;
  public uploadStatus: UploadStatus | null = null;

  constructor(
    private apiTransaction: ApiTransaction
  ) { }

  ngOnInit(): void {
    
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.selectedFile = input.files[0];
      this.uploadStatus = null;
      this.uploadProgress = 0;
      
      // Validar tipo de arquivo
      if (!this.isValidFileType(this.selectedFile)) {
        this.uploadStatus = {
          type: 'error',
          message: 'Tipo de arquivo inválido. Selecione um arquivo .txt ou .cnab'
        };
        this.selectedFile = null;
        return;
      }

      // Validar tamanho do arquivo (max 10MB)
      if (this.selectedFile.size > 10 * 1024 * 1024) {
        this.uploadStatus = {
          type: 'error',
          message: 'Arquivo muito grande. Tamanho máximo: 10MB'
        };
        this.selectedFile = null;
        return;
      }
    }
  }

  private isValidFileType(file: File): boolean {
    const validExtensions = ['.txt', '.cnab'];
    const fileName = file.name.toLowerCase();
    return validExtensions.some(ext => fileName.endsWith(ext));
  }

  async uploadFile(): Promise<void> {
    if (!this.selectedFile) {
      return;
    }

    this.isUploading = true;
    this.uploadProgress = 0;
    this.uploadStatus = null;

    const formData = new FormData();
    formData.append('file', this.selectedFile);

    try {
      const uploadObservable = await this.apiTransaction.Upload(formData);
      
      uploadObservable.subscribe({
        next: (event) => {
          if (event.type === HttpEventType.UploadProgress) {
            // Calcular progresso
            if (event.total) {
              this.uploadProgress = Math.round(100 * event.loaded / event.total);
            }
          } else if (event instanceof HttpResponse) {
            // Upload completo
            this.uploadStatus = {
              type: 'success',
              message: 'Arquivo enviado com sucesso!'
            };
            this.uploadProgress = 100;
          }
        },
        error: (error) => {
          console.error('Erro no upload:', error);
          this.uploadStatus = {
            type: 'error',
            message: `Erro ao enviar arquivo: ${error.error?.message || error.message || 'Erro desconhecido'}`
          };
        },
        complete: () => {
          this.isUploading = false;
        }
      });
    } catch (error) {
      console.error('Erro ao inicializar upload:', error);
      this.uploadStatus = {
        type: 'error',
        message: 'Erro ao inicializar upload'
      };
      this.isUploading = false;
    }
  }

  clearFile(): void {
    this.selectedFile = null;
    this.uploadProgress = 0;
    this.uploadStatus = null;
    
    // clear
    const fileInput = document.getElementById('fileInput') as HTMLInputElement;
    if (fileInput) {
      fileInput.value = '';
    }
  }

  formatFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
}
