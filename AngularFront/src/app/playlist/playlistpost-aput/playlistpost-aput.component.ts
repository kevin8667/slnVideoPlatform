import { Component, ChangeDetectorRef } from '@angular/core';
import { PlaylistDTO } from '../../interfaces/PlaylistDTO';
import { PlaylistitemDTO } from '../../interfaces/PlaylistitemDTO';
import { MemberInfoDTO } from '../../interfaces/MemberInfoDTO';
import { PlaylistService } from '../../services/playlist.service';
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';

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
  playlistItems: PlaylistitemDTO[] = [];
  playlistCollaborators: MemberInfoDTO[] = [];
  availableCollaborators: MemberInfoDTO[] = [];

  imagePreview: string | ArrayBuffer | null = '/assets/img/noimageooo.jpg';

  constructor(private cdr: ChangeDetectorRef, private playlistService: PlaylistService) {}

  getImagePreview(): string {
    return this.imagePreview ? this.imagePreview as string : '/assets/img/noimageooo.jpg';
  }

  showDialog(isEditing: boolean, playlist?: PlaylistDTO): void {
    this.isEditing = isEditing;
    this.displayDialog = true;

    if (isEditing && playlist) {
      this.playlist = { ...playlist };
      this.imagePreview = this.playlist.showImage ? 'data:image/png;base64,' + this.playlist.showImage : '/assets/img/noimageooo.jpg';
      this.loadCollaborators();
      this.loadPlaylistItems();
    } else {
      this.resetForm();
      this.loadCollaborators();
    }
    this.cdr.detectChanges();
  }

  loadCollaborators(): void {
    console.log('協作者加載開始，當前模式:', this.isEditing ? '編輯' : '新增');

    if (this.isEditing && typeof this.playlist.playListId === 'number') {
      this.playlistService.getCollaborators(this.playlist.playListId).subscribe(
        (collaborators) => {
          console.log('協作者資料:', collaborators);
          this.availableCollaborators = collaborators;
        },
        (error) => {
          console.error('Error loading collaborators', error);
        }
      );
    } else {
      this.playlistService.getCollaborators().subscribe(
        (collaborators) => {
          this.availableCollaborators = collaborators.filter(c => c.memberId !== 5);
          console.log('過濾後的協作者:', this.availableCollaborators);
        },
        (error) => {
          console.error('Error loading collaborators', error);
        }
      );
    }
  }

  onImageError(event: any) {
    event.target.src = '/assets/img/memberooo.png';
  }






  loadPlaylistItems(): void {
    // 假設有 API 來獲取播放清單的影片
    this.playlistService.getPlaylistItems(this.playlist.playListId??0).subscribe(items => {
      this.playlistItems = items;
    });
  }

  resetForm(): void {
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
    this.playlistItems = [];
    this.playlistCollaborators = [];
  }

  onCancel(): void {
    this.displayDialog = false;
  }

  onSubmit(): void {
    if (this.playlist.playListName.trim() === '' || this.playlist.playListDescription.trim() === '') {
      alert('請填寫所有必要的欄位');
      return;
    }

    // 提取協作者的 memberId
    const collaboratorIds = this.playlistCollaborators.map(c => c.memberId);

    // 構建要傳遞給後端的 DTO
    const playlistDTO = {
      ...this.playlist,
      collaboratorIds: collaboratorIds
    };

    if (this.isEditing) {
      this.playlistService.editPlaylist(this.playlist.playListId ?? 0, playlistDTO).subscribe(
        (response) => {
          console.log('播放清單已更新:', response);
        },
        (error) => {
          console.error('Error editing playlist', error);
        }
      );
    } else {
      this.playlistService.addNewPlaylist(playlistDTO).subscribe(
        (response) => {
          console.log('播放清單已新增:', response);
        },
        (error) => {
          console.error('Error adding playlist', error);
        }
      );
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

  // 刪除播放清單中的影片
  removePlaylistItem(index: number): void {
    this.playlistItems.splice(index, 1);
  }

  // 拖曳事件處理
  drop(event: CdkDragDrop<PlaylistitemDTO[]>): void {
    moveItemInArray(this.playlistItems, event.previousIndex, event.currentIndex);
  }
}

