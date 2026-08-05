import { Component, input, output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { MatIcon } from '@angular/material/icon';
import { MatNavList, MatListItem } from '@angular/material/list';

export interface MenuItem {
  icon: string;
  label: string;
  route: string;
}

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [RouterLink, RouterLinkActive, MatIcon, MatNavList, MatListItem],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.scss'
})
export class Sidebar {
  // Recibe la configuración desde el layout padre
  readonly menuItems = input.required<MenuItem[]>();
  
  // Emite un evento para que el padre cierre el menú (útil en celulares)
  readonly closeSidebar = output<void>();

  onItemClick() {
    this.closeSidebar.emit();
  }
}
