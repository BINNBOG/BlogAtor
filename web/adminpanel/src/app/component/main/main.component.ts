import { Component, OnInit } from '@angular/core';
import { ChangeNotificationService } from '../../service/API/change-notification.service';
import { UserRole } from '../../model/IUser';
import { AuthenticationService } from '../../service/authentication.service';

@Component({
  selector: 'app-main',
  standalone: false,
  templateUrl: './main.component.html',
  styleUrl: './main.component.css'
})
export class MainComponent implements OnInit {
  protected adminRole = UserRole.Admin;
  protected editorRole = UserRole.Editor;
  protected viewerRole = UserRole.Viewer;

  constructor(private authService: AuthenticationService) {
    
  }

  async ngOnInit() {
    await this.authService.getMeInfo();
  }
}
