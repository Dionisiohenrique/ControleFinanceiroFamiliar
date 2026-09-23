import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../core/auth/auth.service';
import { ButtonComponent } from '../../shared/components/button.component';

@Component({
  selector: 'ff-register',
  standalone: true,
  imports: [FormsModule, RouterLink, ButtonComponent],
  template: `
    <div class="min-h-screen grid place-items-center p-6">
      <div class="card w-full max-w-sm">
        <h1 class="text-2xl font-semibold mb-1">Criar família</h1>
        <p class="text-sm text-neutral-500 mb-6">Comece a organizar as finanças de casa</p>

        <form (ngSubmit)="submit()" class="space-y-3">
          <input class="input" placeholder="Nome da família"  [(ngModel)]="familyName" name="familyName" required />
          <input class="input" placeholder="Seu nome"         [(ngModel)]="ownerName"  name="ownerName" required />
          <input class="input" type="email" placeholder="Email" [(ngModel)]="email"    name="email" required />
          <input class="input" type="password" placeholder="Senha (mín. 6)" [(ngModel)]="password" name="password" required />

          @if (error()) { <p class="text-danger text-sm">{{ error() }}</p> }

          <ff-button [disabled]="loading()">
            {{ loading() ? 'Criando...' : 'Criar conta' }}
          </ff-button>
        </form>

        <p class="text-sm text-center mt-4 text-neutral-500">
          Já tem conta?
          <a routerLink="/login" class="text-accent font-medium">Entrar</a>
        </p>
      </div>
    </div>`
})
export class RegisterComponent {
  private readonly auth = inject(AuthService);
  private readonly router = inject(Router);

  familyName = ''; ownerName = ''; email = ''; password = '';
  readonly loading = signal(false);
  readonly error = signal<string | null>(null);

  submit() {
    this.loading.set(true);
    this.error.set(null);
    this.auth.registerFamily({
      familyName: this.familyName,
      ownerName: this.ownerName,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => this.router.navigateByUrl('/dashboard'),
      error: (err) => { this.error.set(err?.error?.errors?.[0] ?? 'Falha ao registrar'); this.loading.set(false); }
    });
  }
}