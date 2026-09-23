import { CurrencyPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { API_URL } from '../../../core/api';
import { AuthService } from '../../../core/auth/auth.service';
import { ButtonComponent } from '../../shared/components/button.component';
import { CardComponent } from '../../shared/components/card.component';

interface Member { id: string; name: string; email: string; role: string; isActive: boolean; monthExpense: number; }

@Component({
  selector: 'ff-members',
  standalone: true,
  imports: [FormsModule, CardComponent, ButtonComponent, CurrencyPipe],
  template: `
    <header class="mb-6 flex items-center justify-between">
      <div>
        <h1 class="text-2xl font-semibold">Membros</h1>
        <p class="text-sm text-neutral-500">Quem participa da família</p>
      </div>
      @if (auth.user()?.role === 'Owner') {
        <ff-button (click)="showInvite.set(!showInvite())">
          {{ showInvite() ? 'Fechar' : '+ Convidar' }}
        </ff-button>
      }
    </header>

    @if (showInvite()) {
      <ff-card>
        <form (ngSubmit)="invite()" class="grid grid-cols-1 md:grid-cols-3 gap-3">
          <input class="input" placeholder="Nome"  [(ngModel)]="form.name"     name="name" required />
          <input class="input" type="email" placeholder="Email" [(ngModel)]="form.email"    name="email" required />
          <input class="input" type="password" placeholder="Senha temporária" [(ngModel)]="form.password" name="password" required />
          <div class="md:col-span-3 flex justify-end">
            <ff-button [disabled]="saving()">Convidar</ff-button>
          </div>
        </form>
      </ff-card>
    }

    <section class="mt-6 grid grid-cols-1 sm:grid-cols-2 gap-3">
      @for (m of members(); track m.id) {
        <ff-card>
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 rounded-full bg-accent/10 grid place-items-center font-semibold text-accent">
              {{ m.name.charAt(0).toUpperCase() }}
            </div>
            <div class="flex-1">
              <div class="font-medium">{{ m.name }} <span class="text-xs text-neutral-500">· {{ m.role }}</span></div>
              <div class="text-xs text-neutral-500 truncate">{{ m.email }}</div>
            </div>
          </div>
          <div class="mt-3 text-sm">
            <span class="text-neutral-500">Gasto no mês: </span>
            <span class="font-semibold">{{ m.monthExpense | currency:'BRL' }}</span>
          </div>
        </ff-card>
      }
    </section>`
})
export class MembersComponent {
  private readonly http = inject(HttpClient);
  readonly auth = inject(AuthService);

  readonly members = signal<Member[]>([]);
  readonly showInvite = signal(false);
  readonly saving = signal(false);
  form = { name: '', email: '', password: '' };

  constructor() { this.load(); }

  load() {
    this.http.get<Member[]>(`${API_URL}/members`).subscribe(m => this.members.set(m));
  }

  invite() {
    this.saving.set(true);
    this.http.post<Member>(`${API_URL}/members/invite`, this.form).subscribe({
      next: () => { this.saving.set(false); this.showInvite.set(false); this.form = { name: '', email: '', password: '' }; this.load(); },
      error: () => this.saving.set(false)
    });
  }
}