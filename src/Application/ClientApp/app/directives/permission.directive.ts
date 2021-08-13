import { Directive, Input, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Directive({
    selector: '[permission]'
})
export class PermissionDirective {
    constructor(
        private templateRef: TemplateRef<any>,
        private viewContainer: ViewContainerRef,
        private authService: AuthService) { }

    @Input() set permission(roles: string[]) {
        let hasRole = this.authService.hasRole(roles);
        if (hasRole) {
            this.viewContainer.createEmbeddedView(this.templateRef);
        } else {
            this.viewContainer.clear();
        }
    }
}