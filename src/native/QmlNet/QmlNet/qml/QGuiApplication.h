#ifndef NET_QGUIAPPLICATION_H
#define NET_QGUIAPPLICATION_H

#include <QmlNet.h>
#include <QGuiApplication>
#include <QSharedPointer>

typedef void (*guiThreadTriggerCb)();

class GuiThreadContextTriggerCallback : public QObject {
    Q_OBJECT
public:
    GuiThreadContextTriggerCallback();
    guiThreadTriggerCb callback;
public slots:
    void trigger();
};

struct QGuiApplicationContainer {
    int argCount;
    QList<QString> args;
    std::vector<char*> argsPointer;
    QSharedPointer<QGuiApplication> guiApp;
    QSharedPointer<GuiThreadContextTriggerCallback> callback;
};

#endif // NET_QGUIAPPLICATION_H
