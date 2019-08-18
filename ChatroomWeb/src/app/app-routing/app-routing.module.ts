import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ChatComponent } from '../chat/chat.component';
import { LoginComponent } from '../login/login.component';
import { AuthGuard } from '../guards/auth-guard.service';

    const routes: Routes = [
        { path: 'login', component: LoginComponent },
        { path: '', component: ChatComponent, canActivate:[AuthGuard]},
    ];

    @NgModule({
        imports: [
            RouterModule.forRoot(routes)
        ],        
        exports: [
            RouterModule
        ],
        declarations: []
    })
    export class AppRoutingModule { }