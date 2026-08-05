import { Component, Input, Output, EventEmitter, computed, signal, OnInit } from '@angular/core';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-circular-item',
  standalone: true,
  imports: [CommonModule, MatMenuModule, MatIconModule, MatButtonModule],
  templateUrl: './circular-item.html',
  styleUrl: './circular-item.scss',
})
export class CircularItem implements OnInit {
  @Input({ required: true }) name!: string;
  @Input() imageUrl?: string;
  @Input() iconName?: string;
  @Input() color?: string;

  @Output() edit = new EventEmitter<void>();
  @Output() delete = new EventEmitter<void>();
  @Output() selectItem = new EventEmitter<void>();

  // Calcular la inicial o color estático si no hay color provisto
  readonly displayColor = computed(() => {
    if (this.color) return this.color;
    // Generar un color basado en el nombre (hash simple) para que siempre sea el mismo para ese nombre
    let hash = 0;
    for (let i = 0; i < this.name.length; i++) {
      hash = this.name.charCodeAt(i) + ((hash << 5) - hash);
    }
    const h = Math.abs(hash) % 360;
    return `hsl(${h}, 65%, 60%)`;
  });

  readonly firstLetter = computed(() => {
    return this.name ? this.name.charAt(0).toUpperCase() : '?';
  });

  ngOnInit(): void {}
}
