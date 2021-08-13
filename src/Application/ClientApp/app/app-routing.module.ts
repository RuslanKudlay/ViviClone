import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { OnlineChatComponent } from './components/online-chat/online-chat.component';
import { AuthGuardService } from './services/auth-guard.service';
import { LoginComponent } from './components/login/login.component';
import { MainComponent } from './components/main/main.component';

const routes: Routes = [
    { path: 'login', component: LoginComponent },
    {
        path: '',
        component: MainComponent, 
        canActivate: [AuthGuardService],
        canActivateChild: [AuthGuardService],
        children: [
            { path: 'home', component: HomeComponent, canActivate: [AuthGuardService] },
            { path: 'brands', loadChildren: () => import('./components/brand/brand.module').then(mod => mod.BrandModule)  },
            { path: 'promotions', loadChildren: () => import('./components/promotion/promotion.module').then(mod => mod.PromotionModule) },
            { path: 'announcements', loadChildren: () => import('./components/announcement/announcement.module').then(mod => mod.AnnouncementModule) },
            { path: 'posts', loadChildren: () => import('./components/post/post.module').then(mod => mod.PostModule)  },
            { path: 'categories', loadChildren: () => import('./components/category/category.module').then(mod => mod.CategoryModule) },
            { path: 'group-of-wares', loadChildren: () => import('./components/group-of-wares/group-of-wares.module').then(mod => mod.GroupOfWaresModule) },
            { path: 'wares-category-values', loadChildren: () => import('./components/wares-category-values/wares-category-values.module').then(mod => mod.WaresCategoryValuesModule) },
            { path: 'orders', loadChildren: () => import('./components/order/order.module').then(mod => mod.OrderModule)  },
            { path: 'wares', loadChildren: () => import('./components/ware/ware.module').then(mod => mod.WareModule)  },
            { path: 'users', loadChildren: () => import('./components/user/user.module').then(mod => mod.UserModule)  },
            { path: 'sliders', loadChildren: () => import('./components/slider/slider.module').then(mod => mod.SliderModule)  },
            { path: 'chat', component: OnlineChatComponent },
            { path: '**', redirectTo: 'home', pathMatch: 'full' },
        ]
    },
];

@NgModule({
    imports: [
        RouterModule.forRoot(routes)
    ],
    exports: [RouterModule]
})
export class AppRoutingModule { }