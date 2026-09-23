import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../auth/auth.service';
import { ThemeService } from '../theme/theme.service';


@Component({
    selector: 'ff-shell',
    standalone: true,
    imports: [RouterOutlet, RouterLink, RouterLinkActive],
    template: `
    <div class="min-h-screen flex">
    <!-- Sidebar -->
    <aside class="hidden md:flex w-60 flex-col border-r borderblack/5 dark:border-white/5 p-4 gap-1">
        <div class="text-x1 font-semibold px-3 py-4">🪙 FamilyFinance</div>
        <a router-link="/dashboard"     routerLinkActive="bg-black/5 dark:bg-white/5" class="rounded-xl px-3 py-2">DashBoard</a>
        <a routerLink="/transactions" routerLinkActive="bg-black/5 dark:bg-white/5" class="rounded-xl px-3 py-2">Transações</a>
        <a routerLink="/members"      routerLinkActive="bg-black/5 dark:bg-white/5" class="rounded-xl px-3 py-2">Membros</a>
        <a routerLink="/reports"      routerLinkActive="bg-black/5 dark:bg-white/5" class="rounded-xl px-3 py-2">Relatórios</a>

        <div class="mt-auto flex items-center justify-between px-2 pt-4 border-t border-black/5 dark:border-white/5">
            <div class="text-sm-truncate">{{ auth.user()?.name}}</div>
            <div class="flex gap-1">
                <button class="btn-ghost !px-2 !py-1" (click)="theme.toggle()" title="Tema">
                    {{ theme.theme() === 'dark' ? '☀️' : '🌙' }}
                </button>
                <button class="btn-ghost !px-2 !py-1" (click)="auth.logout()" title="Sair">⎋</button>
            </div>
        </div>
    </aside>            
     <!-- Conteúdo -->
      <main class="flex-1 p-4 md:p-8 max-w-5xl mx-auto w-full">
        <router-outlet />
      </main>
    </div>
    `
})
export class ShellComponent {
  readonly auth = inject(AuthService);
  readonly theme = inject(ThemeService);
}