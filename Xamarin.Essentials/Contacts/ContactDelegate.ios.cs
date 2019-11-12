﻿using System;
using Contacts;
using ContactsUI;

namespace Xamarin.Essentials
{
    public class ContactSaveDelegate : CNContactViewControllerDelegate
    {
        public ContactSaveDelegate()
        {
        }

        public ContactSaveDelegate(IntPtr handle)
            : base(handle)
        {
        }

        public override void DidComplete(CNContactViewController viewController, CNContact contact)
        {
            viewController.DismissModalViewController(true);
        }
    }

    public class ContactPickerDelegate : CNContactPickerDelegate
    {
        public ContactPickerDelegate()
        {
        }

        public ContactPickerDelegate(IntPtr handle)
            : base(handle)
        {
        }

        public override void ContactPickerDidCancel(CNContactPickerViewController picker)
        {
            Console.WriteLine("User canceled picker");
            Contact.CallBack(default);
            picker.DismissModalViewController(true);
        }

        public override void DidSelectContact(CNContactPickerViewController picker, CNContact contact)
        {
            Console.WriteLine("Selected: {0}", contact);
            Contact.CallBack(Contact.GetContact(contact));
            picker.DismissModalViewController(true);
        }

        public override void DidSelectContactProperty(CNContactPickerViewController picker, CNContactProperty contactProperty)
        {
            Console.WriteLine("Selected Property: {0}", contactProperty);

            picker.DismissModalViewController(true);
        }
    }
}