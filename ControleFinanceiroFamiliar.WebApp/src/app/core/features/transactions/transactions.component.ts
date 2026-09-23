import { CurrencyPipe, DatePipe } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { API_URL } from '../../../core/api';
import { CardComponent } from '../../shared/components/card.component';
import { ButtonComponent } from '../../shared/components/button.component';

interface Category { id: string; name: string; type: 'Income' | 'Expense'; color: string; }
interface Transaction {
  id: string; amount: number; type: 'Income' | 'Expense';
  categoryId: string; categoryName: string; categoryColor: string;
  memberId: string; memberName: string; date: string; description?: string;
}

@Component({
  selector: 'ff-transactions',
  standalone: true,
  imports: [FormsModule, CardComponent, ButtonComponent, CurrencyPipe, DatePipe],
  template: `
    <header class="mb-6 flex items-center justify-between">
      <div>
        <h1 class="text-2xl font-semibold">Transações</h1>
        <p class="text-sm text-neutral-500">Lançamentos da família</p>
      </div>
      <ff-button (click)="showForm.set(!showForm())">
        {{ showForm() ? 'Fechar' : '+ Nova' }}
      </ff-button>
    </header>

    @if (showForm()) {
      <ff-card>
        <form (ngSubmit)="create()" class="grid grid-cols-1 md:grid-cols-2 gap-3">
          <input class="input" type="number" step="0.01" placeholder="Valor" [(ngModel)]="form.amount" name="amount" required />
          <select class="input" [(ngModel)]="form.type" name="type">
            <option value="Expense">Despesa</option>
            <option value="Income">Receita</option>
          </select>
          <select class="input" [(ngModel)]="form.categoryId" name="categoryId" required>
            <option value="" disabled>Selecione categoria</option>
            @for (c of filteredCategories(); track c.id) {
              <option [value]="c.id">{{ c.name }}</option>
            }
          </select>
          <input class="input" type="date" [(ngModel)]="form.date" name="date" required />
          <input class="input md:col-span-2" placeholder="Descrição (opcional)" [(ngModel)]="form.description" name="description" />
          <div class="md:col-span-2 flex justify-end">
            <ff-button [disabled]="saving()">{{ saving() ? 'Salvando...' : 'Salvar' }}</ff-button>
          </div>
        </form>
      </ff-card>
    }

    <section class="mt-6 space-y-2">
      @for (t of items(); track t.id) {
        <ff-card>
          <div class="flex items-center justify-between">
            <div class="flex items-center gap-3">
              <div class="w-2 h-8 rounded-full" [style.background]="t.categoryColor"></div>
              <div>
                <div class="font-medium">{{ t.description || t.categoryName }}</div>
                <div class="text-xs text-neutral-500">
                  {{ t.memberName }} · {{ t.date | date:'dd/MM/yyyy' }}
                </div>
              </div>
            </div>
            <div class="text-right">
              <div class="font-semibold"
                   [class.text-success]="t.type === 'Income'"
                   [class.text-danger]="t.type === 'Expense'">
                {{ t.type === 'Income' ? '+' : '-' }}{{ t.amount | currency:'BRL' }}
              </div>
              <button class="text-xs text-neutral-500 hover:text-danger mt-1" (click)="remove(t.id)">excluir</button>
            </div>
          </div>
        </ff-card>
      } @empty {
        <ff-card><div class="text-sm text-neutral-500">Nenhuma transação ainda.</div></ff-card>
      }
    </section>`
})
export class TransactionsComponent {
  private readonly http = inject(HttpClient);

  readonly items = signal<Transaction[]>([]);
  readonly categories = signal<Category[]>([]);
  readonly showForm = signal(false);
  readonly saving = signal(false);

  form = {
    amount: 0,
    type: 'Expense' as 'Income' | 'Expense',
    categoryId: '',
    date: new Date().toISOString().slice(0, 10),
    description: ''
  };

  filteredCategories() {
    return this.categories().filter(c => c.type === this.form.type);
  }

  constructor() {
    this.load();
    this.http.get<Category[]>(`${API_URL}/categories`).subscribe(c => this.categories.set(c));
  }

  load() {
    this.http.get<Transaction[]>(`${API_URL}/transactions`).subscribe(t => this.items.set(t));
  }

  create() {
    this.saving.set(true);
    this.http.post<Transaction>(`${API_URL}/transactions`, {
      amount: Number(this.form.amount),
      type: this.form.type,
      categoryId: this.form.categoryId,
      date: this.form.date,
      description: this.form.description || null
    }).subscribe({
      next: () => {
        this.saving.set(false);
        this.showForm.set(false);
        this.form.amount = 0; this.form.description = '';
        this.load();
      },
      error: () => this.saving.set(false)
    });
  }

  remove(id: string) {
    if (!confirm('Excluir esta transação?')) return;
    this.http.delete(`${API_URL}/transactions/${id}`).subscribe(() => this.load());
  }
}