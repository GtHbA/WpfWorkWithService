using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfWorkWithService
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        Friend selectedFriend;  // выбранный друг
        public ObservableCollection<Friend> Friends { get; set; }
        FriendsService friendsService = new FriendsService();
        public event PropertyChangedEventHandler PropertyChanged;

        public StringBuilder StringBuilder { get; set; }

        public ApplicationViewModel()
        {
            Friends = new ObservableCollection<Friend>();
        }

        public Friend SelectedFriend
        {
            get { return selectedFriend; }
            set
            {
                if (selectedFriend != value)
                {
                    Friend tempFriend = new Friend()
                    {
                        Id = value.Id,
                        Name = value.Name,
                        Email = value.Email,
                        Phone = value.Phone
                    };
                    selectedFriend = null;
                }
            }
        }

        public async Task GetFriends()
        {
            
            IEnumerable<Friend> friends = await friendsService.Get();

            // очищаем список
            //Friends.Clear();
            while (Friends.Any())
                Friends.RemoveAt(Friends.Count - 1);

            // добавляем загруженные данные
            foreach (Friend f in friends)
                Friends.Add(f);

        }

        private async void SaveFriend(object friendObject)
        {
            Friend friend = friendObject as Friend;
            if (friend != null)
            {
                if (friend.Id > 0)
                {
                    Friend updatedFriend = await friendsService.Update(friend);
                    // заменяем объект в списке на новый
                    if (updatedFriend != null)
                    {
                        int pos = Friends.IndexOf(updatedFriend);
                        Friends.RemoveAt(pos);
                        Friends.Insert(pos, updatedFriend);
                    }
                }
                // добавление
                else
                {
                    Friend addedFriend = await friendsService.Add(friend);
                    if (addedFriend != null)
                        Friends.Add(addedFriend);
                }
            }
        }
        private async void DeleteFriend(object friendObject)
        {
            Friend friend = friendObject as Friend;
            if (friend != null)
            {
                Friend deletedFriend = await friendsService.Delete(friend.Id);
                if (deletedFriend != null)
                {
                    Friends.Remove(deletedFriend);
                }
            }
        }
    }
}
