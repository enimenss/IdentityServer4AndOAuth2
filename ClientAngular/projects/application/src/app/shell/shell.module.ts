import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import {MatToolbarModule} from '@angular/material/toolbar';
import { ShellComponent } from './shell/shell.component';
import {MatIconModule} from '@angular/material/icon';
import { RouterModule } from '@angular/router';
import {MatButtonModule} from '@angular/material/button';


@NgModule({
  declarations: [ShellComponent],
  imports: [
    CommonModule,
    MatToolbarModule,
    MatIconModule,
    MatButtonModule,
    RouterModule.forChild([
      { path: 'shell', component: ShellComponent}
    ])
  ]
})
export class ShellModule { }
