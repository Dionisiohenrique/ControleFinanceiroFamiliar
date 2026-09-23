import { Component, Input } from '@angular/core';

@Component({
    selector: 'ff-button',
    standalone: true,
    template: `
    <button [class]="classes" [disabled]="disabled" type="button">
    <ng-content/>
    </button>
    `
})
export class ButtonComponent {
    @Input() variant: 'primary' | 'ghost' = 'primary';
    @Input() disabled = false;
    get classes(){
        return this.variant === 'primary' ? 'btn-primary' : 'btn-ghost';
    }
}