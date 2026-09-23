import { CurrencyPipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { API_URL } from '../../../core/api';
import { CardComponent } from '../../shared/components/card.component';

interface MemberReport { memberId: string; memberName: string; income: number; expense: number; balance: number; }
interface CategoryReport { categoryId: string; categoryName: string; color: string; type: 'Income' | 'Expense'; total: number; }

@Component({
  selector: 'ff-reports',
  standalone: true,
  imports: [FormsModule, CardComponent, CurrencyPipe],
  template: `
    <header class="mb-6">
      <h1 class="text-2xl font-semibold">Relatórios</h1>
      <p class="text-sm text-neutral-500">Análise por período</p>
    </header>

    <ff-card>
      <div class="grid grid-cols-2 gap-3">
        <div>
          <label class="text-xs uppercase text-neutral-500">De</label>
          <input class="input mt-1" type="date" [(ngModel)]="from" (change)="load()" />
        </div>
        <div>
          <label class="text-xs uppercase text-neutral-500">Até</label>
          <input class="input mt-1" type="date" [(ngModel)]="to" (change)="load()" />
        </div>
      </div>
    </ff-card>

    <section class="mt-6">
      <h2 class="text-lg font-semibold mb-3">Por membro</h2>
      <div class="space-y-2">
        @for (m of members(); track m.memberId) {
          <ff-card>
            <div class="flex items-center justify-between">
              <div>
                <div class="font-medium">{{ m.memberName }}</div>
                <div class="text-xs text-neutral-500">
                  +{{ m.income | currency:'BRL' }} · -{{ m.expense | currency:'BRL' }}
                </div>
              </div>
              <div class="font-semibold" [class.text-success]="m.balance >= 0" [class.text-danger]="m.balance < 0">
                {{ m.balance | currency:'BRL' }}
              </div>
            </div>
          </ff-card>
        }
      </div>
    </section>

    <section class="mt-6">
      <h2 class="text-lg font-semibold mb-3">Por categoria</h2>
      <div class="space-y-2">
        @for (c of categories(); track c.categoryId) {
          <ff-card>
            <div class="flex items-center justify-between">
              <div class="flex items-center gap-3">
                <div class="w-2 h-6 rounded-full" [style.background]="c.color"></div>
                <div>
                  <div class="font-medium">{{ c.categoryName }}</div>
                  <div class="text-xs text-neutral-500">{{ c.type === 'Income' ? 'Receita' : 'Despesa' }}</div>
                </div>
              </div>
              <div class="font-semibold">{{ c.total | currency:'BRL' }}</div>
            </div>
          </ff-card>
        }
      </div>
    </section>`
})
export class ReportsComponent {
  private readonly http = inject(HttpClient);

  readonly members = signal<MemberReport[]>([]);
  readonly categories = signal<CategoryReport[]>([]);

  from = new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString().slice(0, 10);
  to = new Date(new Date().getFullYear(), new Date().getMonth() + 1, 0).toISOString().slice(0, 10);

  constructor() { this.load(); }

  load() {
    this.http.get<MemberReport[]>(`${API_URL}/reports/by-member?from=${this.from}&to=${this.to}`)
      .subscribe(m => this.members.set(m));
    this.http.get<CategoryReport[]>(`${API_URL}/reports/by-category?from=${this.from}&to=${this.to}`)
      .subscribe(c => this.categories.set(c));
  }
}