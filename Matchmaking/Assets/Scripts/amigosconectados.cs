using System;
using System.Collections.Generic;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;

class Program
{
    static void Main()
    {
        // Inicializa la configuración de Firebase con tu archivo de configuración
        FirebaseApp.Create(new AppOptions
        {
            Credential = GoogleCredential.FromFile("ruta-a-tu-archivo-de-configuracion.json"),
        });

        // Conecta a Firebase Authentication
        var auth = FirebaseAuth.DefaultInstance;

        // Supongamos que tienes una lista de amigos en Firebase Realtime Database
        // y cada amigo tiene un campo "conectado"
        // Aquí obtendremos la lista de amigos conectados
        var database = FirebaseDatabase.DefaultInstance;
        var amigosRef = database.GetReference("amigos");

        var amigosConectados = new List<string>();

        amigosRef.OrderByChild("conectado").EqualTo(true).AddValueEventListener(new ValueEventListenerAdapter
        {
            ChildAdded = (snapshot, previousChildName) =>
            {
                amigosConectados.Add(snapshot.Key);
                Console.WriteLine($"Amigo conectado: {snapshot.Key}");
            },
            /*ChildChanged = (snapshot, previousChildName) =>
            {
                // Manejar cambios de estado aquí
            },*/
            ChildRemoved = (snapshot) =>
            {
                amigosConectados.Remove(snapshot.Key);
                Console.WriteLine($"Amigo desconectado: {snapshot.Key}");
            },
        });

        Console.WriteLine("Presiona Enter para salir...");
        Console.ReadLine();
    }
}