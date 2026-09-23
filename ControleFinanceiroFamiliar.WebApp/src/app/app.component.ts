import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ThemeSerivce } from './core/theme/theme.service';

@Component({
    selector: 'app-root',
    standalone: true,
    imports: [RouterOutlet],
    template: `<router-outlet/>`
})
export class AppComponent{
    private readonly theme = inject(ThemeSerivce); // garante aplicar tema ao inciiar
}