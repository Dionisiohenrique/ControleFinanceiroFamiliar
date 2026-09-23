import { Routes } from '@angular/router';
import { authGuard } from './core/auth/auth.guard';
import { ShellComponent } from './layout/shell.component';

export const routes: Routes = [
    { path: 'login', loadComponent: () => import('./core/features/auth/auth.component').then(m => m.LoginComponent)},
    { path: 'register', loadComponent: () => import('./core/features/auth/register.component').then(m => m.RegisterComponent)},
    {
        path: '',
        component: ShellComponent,
        canActivate: [authGuard],
        children: [
            { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
            { path: 'dashboard', loadComponent: () => import('./core/features/dashboard/dashboard.component').then(m => m.DashboardComponent)},
            { path: 'transactions', loadComponent: () => import('./core/features/transactions/transactions.component').then(m => m.TransactionsComponent)},
            { path: 'members', loadComponent: () => import('./core/features/members/members.component').then(m => m.MembersComponent)},
            { path: 'reports', loadComponent: () => import('./core/features/reports/reports.component').then(m => m.ReportsComponent)}
        ]
    },
    { path: '**', redirectTo: '' }
]