import { effect, Injectable, signal } from '@angular/core';

type Theme = 'light' | 'dark';

@Injectable({ providedIn: 'root'})
export class ThemeService {
    private readonly _theme = signal<Theme>(this.initial());
    readonly theme = this._theme.asReadonly();

    constructor(){
        effect(() => {
            const t = this._theme();
            document.documentElement.classList.toggle('dark', t === 'dark');
            localStorage.setItem('ff.theme', t);
        });
    }

    toggle() { this._theme.update(t => t === 'dark' ? 'light' : 'dark'); }

    private initial(): Theme {
        const saved = localStorage.getItem('ff.theme') as Theme | null;
        if(saved) return saved;
        return matchMedia('(prefers-color-scheme: dark').matches ? 'dark' : 'light';
    }
}
