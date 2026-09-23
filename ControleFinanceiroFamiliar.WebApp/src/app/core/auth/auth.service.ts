import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { API_URL } from '../api';


export interface User {
    id: string;
    name: string;
    email: string;
    role: 'Owner' | 'Member';
    familyId: string;
}

export interface AuthResposne {
    token: string;
    expriesAt: string;
    user: User;
}

const TOKEN_KEY = 'ff.token';
const USER_KEY = 'ff.user';

@Injectable({ providedIn: 'root'})
export class AuthService {
    private readonly http = inject(HttpClient);
    private readonly router = inject(Router);

    private readonly _user = signal<User | null>(this.loadUser());
    readonly user = this._user.asReadonly();
    readonly isAuthenticated = computed(() => this._user() !== null);

    private _token: string | null = localStorage.getItem(TOKEN_KEY);
    get token() { return this._token; }

    login(email: string, password: string){
        return this.http.post<AuthResposne>(`${API_URL}/auth/login`, {email, password}).pipe(tap(res => this.setSession(res)));
    }

    registerFamily(payload: { familyName: string, ownerName: string; email: string; password: string }){
        return this.http.post<AuthResposne>(`${API_URL}/auth/register-family`, payload).pipe(tap(res => this.setSession(res)));
    }

    logout() {
        this._token = null;
        this._user.set(null);
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
        this.router.navigateByUrl('/login');
    }

    private setSession(res: AuthResposne){
        this._token = res.token;
        this._user.set(res.user);
        localStorage.setItem(TOKEN_KEY, res.token);
        localStorage.setItem(USER_KEY, JSON.stringify(res.user));
    }

    private loadUser(): User | null {
        const raw = localStorage.getItem(USER_KEY);
        return raw ? JSON.parse(raw) as User : null;
    }
}