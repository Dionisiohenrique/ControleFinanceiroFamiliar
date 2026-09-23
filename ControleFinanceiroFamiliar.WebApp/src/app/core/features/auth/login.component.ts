import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';
import { ButtonComponent } from '../../shared/components/button.component';

@Component({
  selector: 'ff-login',
  standalone: true,
  imports: [FormsModule, RouterLink, ButtonComponent],
  template: `
    <div class="min-h-screen grid place-items-center p-6">
      <div class="card w-full max-w-sm">
        <h1 class="text-2xl font-semibold mb-1">Entrar</h1>
        <p class="text-sm text-neutral-500 mb-6">Bem-vindo de volta 👋</p>

        <form (ngSubmit)="submit()" class="space-y-3">
          <input class="input" type="email" placeholder="Email" [(ngModel)]="email" name="email" required />
          <input class="input" type="password" placeholder="Senha" [(ngModel)]="password" name="password" required />

          @if (error()) { <p class="text-danger text-sm">{{ error() }}</p> }

          <ff-button [disabled]="loading()">
            {{ loading() ? 'Entrando...' : 'Entrar' }}
          </ff-button>
        </form>

        <p class="text-sm text-center mt-4 text-neutral-500">
          Não tem conta?
          <a routerLink="/register" class="text-accent font-medium">Criar família</a>
        </p>
      </div>
    </div>`
})
export class LoginComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  email = '';
  password = '';
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  submit() {
    this.loading.set(true);
    this.error.set(null);
    this.auth.login(this.email, this.password).subscribe({
      next: () => this.router.navigateByUrl('/dashboard'),
      error: (err) => { this.error.set(err?.error?.errors?.[0] ?? 'Falha ao entrar'); this.loading.set(false); }
    });
  }
}