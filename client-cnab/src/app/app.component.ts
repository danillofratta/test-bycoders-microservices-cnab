import { Component } from '@angular/core';
import { RouterOutlet, RouterLink } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink],
  template: `
    <header style="padding:16px;border-bottom:1px solid #1f2937">
      <nav style="display:flex;gap:16px;align-items:center">
        <h1 style="margin:0;font-size:20px;color:#e5e7eb">CNAB UI</h1>
        <a routerLink="/upload" style="text-decoration:none;color:#22d3ee;padding:8px">Upload</a>
        <a routerLink="/stores" style="text-decoration:none;color:#22d3ee;padding:8px">Lojas</a>
      </nav>
    </header>
    <main style="max-width:960px;margin:24px auto;padding:0 16px">
      <router-outlet></router-outlet>
    </main>
  `
})
export class AppComponent {}
