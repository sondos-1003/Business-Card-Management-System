import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { appRouter } from './app/app-routing.module';

bootstrapApplication(AppComponent, {
  providers: [appRouter()] //call it as a function
});