import { Component } from '@angular/core';

@Component({
    selector: 'ff-card',
    standalone: true,
    template: `<div class="card"><ng-content /></div>`
})
export class CardComponent {}