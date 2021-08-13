import { Directive, ElementRef, Input, Renderer } from '@angular/core';

@Directive({
    selector: '[blink]'
})
export class BlinkDirective {
    @Input() blink: string;

    constructor(private elementRef: ElementRef, private renderer: Renderer) {
        setInterval(() => {
            let style = this.blink;
            if (elementRef.nativeElement.style.color == this.blink) {
                style = 'gray';
            }
            renderer.setElementStyle(elementRef.nativeElement, 'color', style);
        }, 500);
    }
}
