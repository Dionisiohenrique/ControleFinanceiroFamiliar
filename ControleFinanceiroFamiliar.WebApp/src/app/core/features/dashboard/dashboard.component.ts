import { DatePipe, DecimalPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, computed, effect, inject, signal } from '@angular/core';
import { API_URL } from '../../../core/api';
import { AuthService } from '../../../core/auth/auth.service';
import { CardComponent } from '../../shared/components/card.component';

interface Summary { totalIncome: number; totalExpense: number; balance: number; transactionCount: number; }
interface MemberReport { memberId: string; memberName: string; income: number; expense: number; balance: number; }

@Component({
  selector: 'ff-dashboard',
  standalone: true,
  imports: [CardComponent, DecimalPipe, DatePipe],
  template: `
    <header class="mb-6">
      <h1 class="text-2xl font-semibold">Olá, {{ auth.user()?.name }} 👋</h1>
      <p class="text-sm text-neutral-500">Resumo do mês atual</p>
    </header>

    <section class="grid grid-cols-1 sm:grid-cols-3 gap-3 mb-6">
      <ff-card>
        <div class="text-xs uppercase text-neutral-500">Receitas</div>
        <div class="text-2xl font-semibold text-success mt-1">{{ summary()?.totalIncome | number:'1.2-2' }}</div>
      </ff-card>
      <ff-card>
        <div class="text-xs uppercase text-neutral-500">Despesas</div>
        <div class="text-2xl font-semibold text-danger mt-1">{{ summary()?.totalExpense | number:'1.2-2' }}</div>
      </ff-card>
      <ff-card>
        <div class="text-xs uppercase text-neutral-500">Saldo</div>
        <div class="text-2xl font-semibold mt-1"
             [class.text-success]="(summary()?.balance ?? 0) >= 0"
             [class.text-danger]="(summary()?.balance ?? 0) < 0">
          {{ summary()?.balance | number:'1.2-2' }}
        </div>
      </ff-card>
    </section>

    <section>
      <h2 class="text-lg font-semibold mb-3">Gastos por membro</h2>
      <div class="space-y-2">
        @for (m of members(); track m.memberId) {
          <ff-card>
            <div class="flex items-center justify-between">
              <div>
                <div class="font-medium">{{ m.memberName }}</div>
                <div class="text-xs text-neutral-500">
                  Receitas {{ m.income | number:'1.2-2' }} · Despesas {{ m.expense | number:'1.2-2' }}
                </div>
              </div>
              <div class="text-lg font-semibold" [class.text-danger]="m.expense > 0">
                -{{ m.expense | number:'1.2-2' }}
              </div>
            </div>
          </ff-card>
        } @empty {
          <ff-card><div class="text-sm text-neutral-500">Sem movimentações este mês.</div></ff-card>
        }
      </div>
    </section>`
})
export class DashboardComponent {
  private readonly http = inject(HttpClient);
  readonly auth = inject(AuthService);

  readonly summary = signal<Summary | null>(null);
  readonly members = signal<MemberReport[]>([]);

  private readonly period = computed(() => {
    const now = new Date();
    const from = new Date(now.getFullYear(), now.getMonth(), 1);
    const to = new Date(now.getFullYear(), now.getMonth() + 1, 0);
    return { from: from.toISOString().slice(0, 10), to: to.toISOString().slice(0, 10) };
  });

  constructor() {
    effect(() => {
      const { from, to } = this.period();
      this.http.get<Summary>(`${API_URL}/reports/summary?from=${from}&to=${to}`)
        .subscribe(s => this.summary.set(s));
      this.http.get<MemberReport[]>(`${API_URL}/reports/by-member?from=${from}&to=${to}`)
        .subscribe(m => this.members.set(m));
    });
  }
}