#ifndef NETVALUE_H
#define NETVALUE_H

#include <map>

#include <QmlNet.h>
#include <QmlNet/types/NetReference.h>
#include <QmlNet/qml/NetVariantList.h>
#include <QObject>
#include <QSharedPointer>

class NetValueMetaObject;

struct NetValueInterface
{
    virtual ~NetValueInterface() {}
    virtual QSharedPointer<NetReference> getNetReference() = 0;
};

Q_DECLARE_INTERFACE(NetValueInterface, "netcoreqml.NetValueInterface")

class NetValue : public QObject, NetValueInterface
{
    Q_OBJECT
    Q_INTERFACES(NetValueInterface)
public:
    virtual ~NetValue();
    QSharedPointer<NetReference> getNetReference();
    bool activateSignal(const QString& signalName, const QSharedPointer<NetVariantList>& arguments);

    static NetValue* forInstance(const QSharedPointer<NetReference>& instance);
    static QList<NetValue*> getAllLiveInstances(const QSharedPointer<NetReference>& instance);

protected:
    NetValue(const QSharedPointer<NetReference>& instance, QObject *parent);

private:
    QSharedPointer<NetReference> instance;
    NetValueMetaObject* valueMeta;

    struct NetValueCollection {
        QList<NetValue*> netValues;
    };
    static QMap<uint64_t, NetValueCollection*> objectIdNetValuesMap;
};

#endif // NETVALUE_H
