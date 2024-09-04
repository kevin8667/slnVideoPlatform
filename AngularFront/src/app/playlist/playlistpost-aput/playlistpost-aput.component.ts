import { Component, ChangeDetectorRef } from '@angular/core';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';

@Component({
  selector: 'app-playlistpost-aput',
  templateUrl: './playlistpost-aput.component.html',
  styleUrls: ['./playlistpost-aput.component.css']
})
export class PlaylistpostAputComponent {
  isEditing: boolean = false;
  displayDialog: boolean = false;
  playlist: PlaylistDTO = {
    playListId: 0,
    playListName: '',
    playListDescription: '',
    viewCount: 100,
    likeCount: 100,
    addedCount: 100,
    sharedCount: 100,
    showImage: null
  };

  imagePreview: string | ArrayBuffer | null = '/assets/img/noimageooo.jpg';

  constructor(private cdr: ChangeDetectorRef) {}

  getImagePreview(): string {
    return this.imagePreview ? this.imagePreview as string : '/assets/img/noimageooo.jpg';
  }

  showDialog(isEditing: boolean, playlist?: PlaylistDTO): void {
    this.isEditing = isEditing;
    this.displayDialog = true;

    if (isEditing && playlist) {
      this.playlist = { ...playlist };
      this.imagePreview = this.playlist.showImage ? 'data:image/png;base64,' + this.playlist.showImage : '/assets/img/noimageooo.jpg';
    } else {
      this.playlist = {
        playListId: 0,
        playListName: '',
        playListDescription: '',
        viewCount: 100,
        likeCount: 100,
        addedCount: 100,
        sharedCount: 100,
        showImage: null
      };
      this.imagePreview = '/assets/img/noimageooo.jpg';
    }
    this.cdr.detectChanges();
  }

  onCancel(): void {
    this.displayDialog = false;
  }

  onSubmit(): void {
    if (this.playlist.playListName.trim() === '' || this.playlist.playListDescription.trim() === '') {
      alert('請填寫所有必要的欄位');
      return;
    }

    if (this.isEditing) {
      console.log('編輯播放清單:', this.playlist);
    } else {
      console.log('新增播放清單:', this.playlist);
    }

    this.displayDialog = false;
  }

  onImageSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.imagePreview = reader.result;

        this.cdr.detectChanges();
      };
      reader.readAsDataURL(file);
      reader.onloadend = () => {
        this.playlist.showImage = reader.result as string;
        this.cdr.detectChanges();
      };
    }
  }
}

