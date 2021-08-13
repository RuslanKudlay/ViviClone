import { CommonModule } from '@angular/common';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ApplicationRef, NgModule } from '@angular/core';
import { MatTabsModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { AngularDateTimePickerModule } from 'angular2-datetimepicker';
import { BootstrapModalModule } from 'ng2-bootstrap-modal';
import { FileUploadModule } from 'ng2-file-upload';
import { AppRoutingModule } from './app-routing.module';
import { DeleteConfirmationModalComponent } from './common/delete-confirmation-modal.component';
import { BlinkDirective } from './common/directives/blink.directive';
import { ErrorModalComponent } from './common/error-modal/error-modal.component';
import { IconModalComponent } from './common/icon-modal.component';
import { ImageModalComponent } from './common/image-modal.component';
import { VideoModalComponent } from './common/video-modal/video-modal.component'
import { CloseChatModalComponent } from './common/modals/close-chat--modal.component';
import { AppComponent } from './components/app/app.component';
import { HomeComponent } from './components/home/home.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { OnlineChatComponent } from './components/online-chat/online-chat.component';
import { AnnouncementService } from './services/announcement.service';
import { BlogService } from './services/blog.service';
import { BrandService } from './services/brand.service';
import { CategoryValuesService } from './services/category-values.service';
import { CategoryService } from './services/category.service';
import { ChatService } from './services/chat.service';
import { CreateCyrillicFriendlySuburlService } from './services/create-cyrillic-friendly-suburl.service';
import { ExtensionModalService } from './services/extension-modal-service';
import { GroupOfWaresService } from './services/group-of-wares.service';
import { ImageService } from './services/image.service';
import { LanguageService } from './services/language.service';
import { OrderService } from './services/order.service';
import { PromotionService } from './services/promotion.service';
import { UploadImageModalService } from './services/upload-image-modal.service';
import { VideoModalService } from './services/video-modal.service';
import { WareService } from './services/ware.service';
import { WaresCategoryValuesService } from './services/wares-category-values.service';
import { SharedModule } from './shared.module';
import { Interceptor } from './services/interceptor.service';
import { AuthGuardService } from './services/auth-guard.service';
import { AuthService } from './services/auth.service';
import { LoginComponent } from './components/login/login.component';
import { MainComponent } from './components/main/main.component';
import { PermissionDirective } from './directives/permission.directive';
import { UserService } from './services/user.service';
import { SliderService } from './services/slider.service';

@NgModule({
    declarations: [
        AppComponent, NavMenuComponent, HomeComponent,
        ImageModalComponent, VideoModalComponent, IconModalComponent,
        DeleteConfirmationModalComponent, CloseChatModalComponent, ErrorModalComponent, OnlineChatComponent, BlinkDirective,
        LoginComponent, MainComponent, PermissionDirective
    ],
    imports: [
        CommonModule,
        MatTabsModule,
        BrowserAnimationsModule,
        HttpClientModule,
        BootstrapModalModule.forRoot({ container: document.body }),
        AngularDateTimePickerModule,
        SharedModule,
        AppRoutingModule,
        FileUploadModule
    ],
    providers: [
        AnnouncementService, BrandService, CreateCyrillicFriendlySuburlService, PromotionService, UploadImageModalService, BlogService,
        CategoryService, WareService, CategoryValuesService, GroupOfWaresService, WaresCategoryValuesService, ExtensionModalService,
        LanguageService, ImageService, OrderService, ChatService, AuthGuardService, AuthService, UserService, SliderService, VideoModalService,
        {
            provide: HTTP_INTERCEPTORS,
            useClass: Interceptor,
            multi: true
        },
    ],
    bootstrap: [AppComponent],
    entryComponents: [
        ImageModalComponent, IconModalComponent, ErrorModalComponent, CloseChatModalComponent, DeleteConfirmationModalComponent, VideoModalComponent
    ]

})

export class AppModule {
    constructor(applicationRef: ApplicationRef) {
        //for ng2-bootstrap-modal in angualar 5+(6, 7 etc..)
        Object.defineProperty(applicationRef, '_rootComponents', { get: () => applicationRef['components'] });
    }
}
